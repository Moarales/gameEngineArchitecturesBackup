#include "Allocator.h"

#include <cassert>
#include <stdexcept>

#include "math.h"

Allocator::Allocator(size_t availableMemorySize, void* availableMemory)
{

    //create new free memory block:
    auto elem = static_cast<Elem*>(availableMemory);

    elem->isFree = true;
    //size exclusive with overhead
    elem->size = availableMemorySize - 5;
    elem->next = nullptr;
    elem->prev = nullptr;


    //move stored memory down 4 bytes: size_t, prev,next, bool
    elem->storedMemory = static_cast<void*>(static_cast<size_t*>(availableMemory)+4);


    auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(availableMemory) +elem->size) - 1;

    //write size of block to end
    *pEndOfMemory = elem->size;


    auto listValue = log2(availableMemorySize);
    int exp = ceil(listValue);

    //store free memory inside lists
    _freeMemory[exp] = elem;
}

void* Allocator::alloc(size_t size)
{
    //size_t neededsize = size + sizeof(size_t) + sizeof(Elem*);
    size_t neededsize = size + 5;


    //find memory block that can fit this size:
    int startIndexOfFreeMemoryList = ceil(log2(neededsize));


    int index = startIndexOfFreeMemoryList;


    Elem* currentFreeEl = nullptr;

    while(index < 64)
    {
        //go to next bigger exp
        if(_freeMemory[index] == nullptr)
        {
	        continue;
        }
        currentFreeEl = _freeMemory[index];
        //find memory that is big enough
	    while(currentFreeEl->size < neededsize)
	    {
            currentFreeEl = currentFreeEl->next;
	    }

        if(currentFreeEl->size >= neededsize)
        {
            break;
        }

        index++;
    }

    if(currentFreeEl->size < neededsize)
    {
        throw std::overflow_error("Couldn't find enought free memory for this object");
    }

    //delete currentElement from Free List
    if(currentFreeEl->prev != nullptr)
    {
        currentFreeEl->prev->next = currentFreeEl->next;
        currentFreeEl->next->prev = currentFreeEl->prev;

    }else
    {
        _freeMemory[index] = nullptr;
    }


    currentFreeEl->size -= neededsize;


    //auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(elem->storedMemory) + elem->size) - 1;

    //pointer to end of the new block + the new size + 1
    auto pointerToNewFreeMemory = reinterpret_cast<size_t*>(static_cast<char*>(currentFreeEl->storedMemory) + currentFreeEl->size + 1);
    auto memoryPointer = reinterpret_cast<void*>(pointerToNewFreeMemory);

    auto endOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(currentFreeEl->storedMemory) + currentFreeEl->size) - 1;

    //write size of block to end
    *endOfMemory = currentFreeEl->size;

    //add new free memory to list
    //sort the new free element 
    AddFreeMemoryElem(currentFreeEl);


    //create new Elem
    auto newAllocatedElem = static_cast<Elem*>(memoryPointer);


    newAllocatedElem->size = size;
    newAllocatedElem->prev = nullptr;
    newAllocatedElem->next = nullptr;
    newAllocatedElem->isFree = false;
    newAllocatedElem->storedMemory = static_cast<void*>(static_cast<size_t*>(memoryPointer) + 4);

    auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(newAllocatedElem->storedMemory) + newAllocatedElem->size) - 1;

    //write size of block to end
    *pEndOfMemory = newAllocatedElem->size;

    return newAllocatedElem->storedMemory;
}

void Allocator::free(void* Data)
{

    auto elemToBeFreed = reinterpret_cast<Elem*>(static_cast<size_t*>(Data) - 4);

    //get prevElement:
    auto prevElemSize = static_cast<size_t*>(Data) - 5;
    auto prevElem = reinterpret_cast<Elem*>(static_cast<size_t*>(Data) - *prevElemSize - 4);

    //get nextElement:
    //auto nextElemSize = reinterpret_cast<size_t*>(static_cast<char*>(elemToBeFreed->storedMemory)+elemToBeFreed->size + 1);
    auto nextElem = reinterpret_cast<Elem*>(static_cast<size_t*>(Data) + elemToBeFreed->size + 1);


    Elem* mergedElem = elemToBeFreed;

    if(prevElem->isFree)
    {
        mergedElem = MergeMemory(prevElem, elemToBeFreed);
    }

    if(nextElem->isFree)
    {
        mergedElem = MergeMemory(mergedElem, nextElem);
    }


    AddFreeMemoryElem(mergedElem);
}

Elem* Allocator::MergeMemory(Elem* prevElement, Elem* followingElement)
{
    if(!prevElement->isFree || !followingElement->isFree)
    {
        throw std::invalid_argument("One storage need to be free");
    }


    //remove element from free memory list
    if(prevElement->isFree)
    {
        RemoveElemFromFreeMemoryList(prevElement);
    }else if(followingElement->isFree)
    {
        RemoveElemFromFreeMemoryList(followingElement);
    }

    auto newSize = prevElement->size + followingElement->size - 5;

    prevElement->size = newSize;
    auto pEndOfMemory = reinterpret_cast<size_t*>(static_cast<char*>(prevElement->storedMemory) + prevElement->size) - 1;
    //write size of block to end
    *pEndOfMemory = prevElement->size;

    return prevElement;
}

int Allocator::FindFittingList(Elem* elem)
{
    return ceil(log2(elem->size+5));

}

void Allocator::RemoveElemFromFreeMemoryList(Elem* elem)
{
        //if first in free list 
        if (elem->prev == nullptr)
        {
            auto index = FindFittingList(elem);
            _freeMemory[index] = elem->next;
            elem->next->prev = nullptr;
        }
        else //if element is in the middle
        {
            elem->prev->next = elem->next;
            if(elem->next != nullptr)
            {
                elem->next->prev = elem->prev;
            }
        }
        elem->next = nullptr;
        elem->prev = nullptr;
}


void Allocator::AddFreeMemoryElem(Elem* elem)
{

    elem->isFree = true;
    elem->next = nullptr;
    elem->prev = nullptr;


    int listIndex = ceil(log2(elem->size));

    auto currentElement = _freeMemory[listIndex];

    if(currentElement == nullptr)
    {
        _freeMemory[listIndex] = elem;
        assert(elem->next == nullptr, "should be ´list wiht single item");
        assert(elem->prev == nullptr, "should be ´list wiht single item");
        return;
    }

	while(currentElement->next != nullptr)
	{
        currentElement = currentElement->next;
	}

    currentElement->next = elem;
    elem->prev = currentElement;

    assert(elem->prev == currentElement);
    assert(elem->next == nullptr);
    
}
