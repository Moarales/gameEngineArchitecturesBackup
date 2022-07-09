// MemoryManager.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#include "Allocator.h"

int main()
{
    const size_t memory = 1024;

    char memoryStorage[memory];
    Allocator myMemory(1024, memoryStorage);

    size_t neededMemory = 10;
    void* pMemory = myMemory.alloc(neededMemory);
    void* pMemory2 = myMemory.alloc(15);
    void* pMemory3 = myMemory.alloc(300);


    myMemory.free(pMemory);
    myMemory.free(pMemory3);
    myMemory.free(pMemory2);

    pMemory = nullptr;


    std::cout << "lmao\n";
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
