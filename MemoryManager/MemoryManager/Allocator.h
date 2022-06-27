#pragma once



struct Elem
{
		size_t size;
		Elem* prev;
		Elem* next;
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
	Elem* _first = nullptr;
	Elem* _last = nullptr;
};