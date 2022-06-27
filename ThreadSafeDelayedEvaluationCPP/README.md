# Task 3
1. **Regeln, unter welchem Umst�nden und in welcher Reihenfolge Mutex-Objekte oder Critical
Sections gelockt werden m�ssen und d�rfen**

Critical Sections werden durch 2 mutex blockiert: **_mutex** und **_mutexRef**, diese Objekte werden genutzt um das Lesen und Schreiben der Member des Nodes sowie den Referenzcounter zu blockieren.

Um die Member abgesehen vom _refCounter zu �ndern, muss das _mutex Objekt gebblockt werden, um den _refCounterzu �ndern muss _mutexRef geblocked werden.



2.**Regeln, welche Mutex-Objekte oder Critical Sections gelockt werden m�ssen, um eine
bestimmte Variable lesend oder schreibend zugreifen zu k�nnen.**

### Critical Sections geblocked mit _mutex:
+ ~Node, setParent, setParam, registerNotification, getResult, registerNotification,unsubscribeNotification
### Critical Sections geblocked mit _mutexRef:
+ addReference, releaseReference, 




3.**Eine theoretische Begr�ndung, dass diese Regelen Thead-Safety sicherstellen und
Deadlocks nachweislich ausschlie�en.**

Dadurch dass zuerst das Objekt mittels _mutex geblocked werden muss um auch den _refCounter blockieren zu k�nnen, kann kein Deadlock auftreten. Thread safety wird sichergestellt durch die beiden mutex Objekte wird sichergestellt dass keine Race Condition die Ausgabe des Ergebnisses verf�lscht. Nodes werden gelocked bei Ver�nderung der Parameter und geben daher keine stale Values zur�ck
