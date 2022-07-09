#include "Allocator.h"

#include <cassert>
#include <stdexcept>
#include <cmath>

Allocator::Allocator(size_t availableMemorySize, void* availableMemory)
{
    
    //create new free memory block:
    auto start = static_cast<size_t*>(availableMemory);
    *start = 0;
    auto elem = reinterpret_cast<Elem*>(static_cast<size_t*>(availableMemory)+1);

    elem->isFree = true;
    //size exclusive with overhead from elem and allocator
    elem->size = availableMemorySize - 2*sizeof(size_t) - sizeof(Elem);
    elem->next = nullptr;
    elem->prev = nullptr;

    auto end = reinterpret_cast<size_t*>(static_cast<char*>(availableMemory) + availableMemorySize) -1 ;
    *end = 0;

    //Start and end of allocatable memory is marked with size = 0;



    //move stored memory down 40 bytes: size_t, prev,next, bool
    elem->storedMemory = static_cast<void*>(reinterpret_cast<size_t *>(static_cast<char*>(availableMemory) + sizeof(Elem)) + 1 );


    auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(availableMemory) + availableMemorySize) - 2;

    //write size of block to end
    *pEndOfMemory = elem->size;


    auto listValue = log2(availableMemorySize);
    int exp = ceil(listValue);

    //store free memory inside lists
    _freeMemory[exp] = elem;
}

void* Allocator::alloc(size_t size)
{

    //40 byte for members and
    const size_t neededsize = size + sizeof(Elem);

    Elem* currentFreeEl = FindFreeElemInList(neededsize);
    RemoveElemFromFreeMemoryList(currentFreeEl);

    size_t newSize = currentFreeEl->size - neededsize;

    if(newSize < sizeof(Elem)+1)
    {
	    //skip splitting
        currentFreeEl->isFree = false;
        return currentFreeEl;
    }


    //split Element
    currentFreeEl->size -= neededsize;
    //pointer to end of the new block + the new size
    const auto pointerToNewFreeMemory = reinterpret_cast<size_t*>(static_cast<char*>(currentFreeEl->storedMemory) + currentFreeEl->size);
    const auto memoryPointer = reinterpret_cast<void*>(pointerToNewFreeMemory);

    const auto endOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(currentFreeEl->storedMemory) + currentFreeEl->size) -1 ;

    //write size of block to end
    *endOfMemory = currentFreeEl->size;

    //add new free memory to list
    //sort the new free element 
    AddFreeMemoryElem(currentFreeEl);

    //create new Elem
    const auto newAllocatedElem = static_cast<Elem*>(memoryPointer);
    newAllocatedElem->size = size;
    newAllocatedElem->prev = nullptr;
    newAllocatedElem->next = nullptr;
    newAllocatedElem->isFree = false;
    newAllocatedElem->storedMemory =static_cast<char*>(memoryPointer) + sizeof(Elem);


    const auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(newAllocatedElem->storedMemory)+newAllocatedElem->size) -1;

    //write size of block to end
    *pEndOfMemory = newAllocatedElem->size;

    return newAllocatedElem->storedMemory;
}

void Allocator::free(void* Data)
{
	const auto elemToBeFreed = reinterpret_cast<Elem*>(static_cast<char*>(Data) - sizeof(Elem));

    //get prevElement:
    auto prevElemSize = reinterpret_cast<size_t*>(static_cast<char*>(Data) - sizeof(Elem)) - 1;


    Elem* prevElem = nullptr;
    //start of memory not reached
    if(*prevElemSize != 0)
    {
        prevElem = reinterpret_cast<Elem*>(static_cast<char*>(Data) - sizeof(Elem) - sizeof(Elem) - *prevElemSize);
    }


    auto nextElemSize = reinterpret_cast<size_t*>(static_cast<char*>(Data) + elemToBeFreed->size);

	Elem* nextElem = nullptr;


    //end of memoryReached not reached
    if(*nextElemSize != 0)
    {
        nextElem = reinterpret_cast<Elem*>(reinterpret_cast<size_t*>(static_cast<char*>(Data) + elemToBeFreed->size));
    }



    Elem* mergedElem = elemToBeFreed;

    if (prevElem && prevElem->isFree)
    {
        mergedElem = MergeElems(prevElem, mergedElem);
    }
    mergedElem->isFree = false;

    if (nextElem && nextElem->isFree)
    {
        mergedElem = MergeElems(mergedElem, nextElem);
    }


    AddFreeMemoryElem(mergedElem);
}

Elem* Allocator::MergeElems(Elem* prevElement, Elem* followingElement)
{
    if (!prevElement->isFree && !followingElement->isFree)
    {
        throw std::invalid_argument("One storage need to be free");
    }


    //remove element from free memory list
    if (prevElement->isFree)
    {
        RemoveElemFromFreeMemoryList(prevElement);
    }
	if (followingElement->isFree)
    {
        RemoveElemFromFreeMemoryList(followingElement);
    }

    const auto newSize = prevElement->size + followingElement->size + sizeof(Elem);

    prevElement->size = newSize;
    auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(prevElement->storedMemory) + prevElement->size) - 1;
    //write size of block to end
    *pEndOfMemory = prevElement->size;

    return prevElement;
}

int Allocator::FindFittingList(const Elem* elem)
{
    return ceil(log2(elem->size + 5));

}

void Allocator::RemoveElemFromFreeMemoryList(Elem* elem)
{
    //if first in free list 
    if (elem->prev == nullptr)
    {
        auto index = FindFittingList(elem);
        if(elem->next)
        {
            _freeMemory[index] = elem->next;
            elem->next->prev = nullptr;
        }else
        {
            _freeMemory[index] = nullptr;
        }
    }
    else //if element is in the middle
    {
        elem->prev->next = elem->next;
        if (elem->next != nullptr)
        {
            elem->next->prev = elem->prev;
        }
    }
    elem->next = nullptr;
    elem->prev = nullptr;

}

Elem* Allocator::FindFreeElemInList(size_t neededSize)
{

    Elem* currentFreeEl = nullptr;
    //find memory block that can fit this size:
    int startIndexOfFreeMemoryList = ceil(log2(neededSize));


    int index = startIndexOfFreeMemoryList;
    while (index < 64)
    {
        //go to next bigger exp
        if (_freeMemory[index] == nullptr)
        {
            index++;
            continue;
        }
        currentFreeEl = _freeMemory[index];
        //find memory that is big enough
        while (currentFreeEl->size < neededSize)
        {
            currentFreeEl = currentFreeEl->next;
        }

        if (currentFreeEl->size >= neededSize)
        {
            break;
        }

        index++;
    }

    if (currentFreeEl->size < neededSize)
    {
        throw std::overflow_error("Couldn't find enough free memory for this object");
    }

    return currentFreeEl;

}


void Allocator::AddFreeMemoryElem(Elem* elem)
{

    elem->isFree = true;
    elem->next = nullptr;
    elem->prev = nullptr;


    int listIndex = ceil(log2(elem->size));

    auto currentElement = _freeMemory[listIndex];

    if (currentElement == nullptr)
    {
        _freeMemory[listIndex] = elem;
        assert(elem->next == nullptr, "should be ´list wiht single item");
        assert(elem->prev == nullptr, "should be ´list wiht single item");
        return;
    }

    while (currentElement->next != nullptr)
    {
        currentElement = currentElement->next;
    }

    currentElement->next = elem;
    elem->prev = currentElement;

    assert(elem->prev == currentElement);
    assert(elem->next == nullptr);

}
