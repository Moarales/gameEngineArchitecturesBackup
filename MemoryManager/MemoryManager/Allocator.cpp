#include "Allocator.h"

Allocator::Allocator(size_t availableMemorySize, void* availableMemory)
{
}

void* Allocator::alloc(size_t size)
{
    //PSEUDO CODE:
    //size_t needessize = size + sizeof(size_t);
    //Elem* elem = first;
    //while (elem && elem->size < neededsize)
    //    elem = elem->next;
    //if (!elem)
    //    return 0;
    //if (elem->size <= neededsize + sizeof(Elem))
    //{
    //    if (elem->prev)
    //        elem->prev->next = elem->next;
    //    else
    //        first = elem->next;
    //    if (elem->next->prev)
    //        elem->next->prev = elem->prev;
    //    else
    //        last = elem_prev;
    //    return (size_t*)elem + 1;
    //}
    //else
    //{
    //    Elem* newElem = (Elem*)((char*)elem + neededsize);
    //    newElem->prev = elem->prev;
    //    newElem->next = elem->next;
    //    newElem->size = elem->size - neededsize;
    //    if (elem->prev)
    //        elem->prev->next = newElem;
    //    else
    //        first = newElem;
    //    if (elem->next)
    //        elem->next->prev = newElem;
    //    else
    //        last = newElem;
    //    *((size_t*)elem) = neededsize;
    //    return (size_t*)elem + 1;
    //}
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
