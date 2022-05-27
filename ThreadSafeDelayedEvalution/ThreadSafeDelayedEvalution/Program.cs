// See https://aka.ms/new-console-template for more information


using ThreadSafeDelayedEvalution;

var node1 = new Node();
node1.addReference();
node1.setParam(1);
var node2 = new Node();
node2.addReference();
node2.setParam(2);
node2.setParent(node1);
var node3 = new Node();
node3.addReference();
node3.setParam(3);
node3.setParent(node2);
var node4 = new Node();
node4.addReference();
node4.setParam(4);
node4.setParent(node3);
int result4 = node4.getResult(); // All nodes are calculated.
node2.setParam(20);
int result3 = node3.getResult(); // Only node2 and node3 are recalculated.
node3.releaseReference(); // node3 wird noch von node4 benutzt.
node4.releaseReference(); // Zerstört node3 und node4.


Console.WriteLine("Press Key to exit Program");
Console.ReadKey();
