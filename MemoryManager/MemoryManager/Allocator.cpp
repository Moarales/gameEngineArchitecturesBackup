#include "Allocator.h"

#include <cassert>
#include <stdexcept>

#include "math.h"

Allocator::Allocator(size_t availableMemorySize, void* availableMemory)
{

    //create new free memory block:
    Elem* elem = new Elem();
    elem->isFree = true;
    //size exclusive with overhead
    elem->size = availableMemorySize;


    auto listValue = log2(availableMemorySize + sizeof(Elem*));
    int exp = ceil(listValue);

    //store free memory inside lists
    _freeMemory[exp] = elem;

}

void* Allocator::alloc(size_t size)
{
    size_t neededsize = size + sizeof(size_t) + sizeof(Elem*);


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


    //add new free memory to list
    AddFreeMemoryElem(currentFreeEl);

    assert(currentFreeEl->next == nullptr);

    //CREATE now allocated memory









    //PSEUDO CODE:
    size_t neededsize = size + sizeof(size_t);

    Elem* elem = _first;
    while (elem && elem->size < needed size)
        elem = elem->next;
    if (!elem)
    {
        return 0;
    }
    if (elem->size <= neededsize + sizeof(Elem))
    {
        if (elem->prev)
            elem->prev->next = elem->next;
        else
            _first = elem->next;
        if (elem->next->prev)
            elem->next->prev = elem->prev;
        else
            _last = elem_prev;
        return (size_t*)elem + 1;
    }
    else
    {
        Elem* newElem = (Elem*)((char*)elem + neededsize);
        newElem->prev = elem->prev;
        newElem->next = elem->next;
        newElem->size = elem->size - neededsize;
        if (elem->prev)
            elem->prev->next = newElem;
        else
            _first = newElem;
        if (elem->next)
            elem->next->prev = newElem;
        else
            _last = newElem;
        *((size_t*)elem) = neededsize;
        return (size_t*)elem + 1;
    }
}

void Allocator::free(void* Data)
{
    //PSEUDO CODE:
    // Missing: Merging!!!
    //Elem* elem = (Elem*)((size_t*)p - 1);
    //elem->prev = last;
    //elem->next = 0;
    //memset(elem + 1, 0, elem->size - sizeof(Elem));
    //if (last)
    //    last->next = elem;
    //else
    //    first = elem;
    //last = elem;
}

void Allocator::AddFreeMemoryElem(Elem* elem)
{

    elem->isFree = true;
    elem->next = nullptr;
    elem->prev = nullptr;

    int listIndex = ceil(log2(elem->size + sizeof(Elem*)));

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
