// ThreadSafeDelayedEvaluationCPP.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#include "Node.h"

int main()
{
    auto node1 = new Node();
    node1->addReference();
    node1->setParam(1);
    auto node2 = new Node();
    node2->addReference();
    node2->setParam(2);
    node2->setParent(node1);
    auto node3 = new Node();
    node3->addReference();
    node3->setParam(3);
    node3->setParent(node2);
    auto node4 = new Node();
    node4->addReference();
    node4->setParam(4);
    node4->setParent(node3);
    int result4 = node4->getResult(); // All nodes are calculated.
    node2->setParam(20);
    int result3 = node3->getResult(); // Only node2 and node3 are recalculated.
    node3->releaseReference(); // node3 wird noch von node4 benutzt.
    node4->releaseReference(); // Zerstört node3 und node4.

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
