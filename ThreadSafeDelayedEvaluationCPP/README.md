# Task 3
1. **Regeln, unter welchem Umständen und in welcher Reihenfolge Mutex-Objekte oder Critical
Sections gelockt werden müssen und dürfen**

Critical Sections werden durch 2 mutex blockiert: **_mutex** und **_mutexRef**, diese Objekte werden genutzt um das Lesen und Schreiben der Member des Nodes sowie den Referenzcounter zu blockieren.

Um die Member abgesehen vom _refCounter zu ändern, muss das _mutex Objekt gebblockt werden, um den _refCounterzu ändern muss _mutexRef geblocked werden.



2.**Regeln, welche Mutex-Objekte oder Critical Sections gelockt werden müssen, um eine
bestimmte Variable lesend oder schreibend zugreifen zu können.**

### Critical Sections geblocked mit _mutex:
+ ~Node, setParent, setParam, registerNotification, getResult, registerNotification,unsubscribeNotification
### Critical Sections geblocked mit _mutexRef:
+ addReference, releaseReference, 




3.**Eine theoretische Begründung, dass diese Regelen Thead-Safety sicherstellen und
Deadlocks nachweislich ausschließen.**

Dadurch dass zuerst das Objekt mittels _mutex geblocked werden muss um auch den _refCounter blockieren zu können, kann kein Deadlock auftreten. Thread safety wird sichergestellt durch die beiden mutex Objekte wird sichergestellt dass keine Race Condition die Ausgabe des Ergebnisses verfälscht. Nodes werden gelocked bei Veränderung der Parameter und geben daher keine stale Values zurück
