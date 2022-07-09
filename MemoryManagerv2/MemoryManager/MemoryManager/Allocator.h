#pragma once



struct Elem
{
	size_t size;
	Elem* prev;
	Elem* next;
	bool isFree;
	void* storedMemory;
};

class Allocator
{
public:
	// Initialize allocator and provide a continuous memory
	//
	// where the allocations is allocated from.
	Allocator(size_t availableMemorySize, void* availableMemory);
	// Allocate a memory block of size Size.
	// Return null if not enough memory is available anymore.
	void* alloc(size_t size);
	// Free a memory block allocated by Alloc.
	void free(void* Data);

private:

	Elem* MergeElems(Elem* prevElement, Elem* followingElement);
	void AddFreeMemoryElem(Elem* elem);
	int FindFittingList(const Elem* elem);
	void RemoveElemFromFreeMemoryList(Elem* elem);
	Elem* FindFreeElemInList(size_t neededSize);

	Elem* _freeMemory[64] = {};

	//void* _memory;
};


