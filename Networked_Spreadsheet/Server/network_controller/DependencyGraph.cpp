#include "DependencyGraph.h"
#include <iostream>
#include <list>
#include <exception>

using namespace std;

/**
 * Dependency Graph Functions
 */


set<string> DependencyGraph::getDependents(std::string s)
{
    std::map<string,set<string> >::iterator it;
    it = dependees.find(s);
    if (it == dependees.end())
    {
        set<std::string> out;
        return out;
    }
    return dependees.find(s)->second;
}

set<string> DependencyGraph::getDependees(std::string s)
{
    std::map<string,set<string> >::iterator it;
    it = dependents.find(s);
    if (it == dependents.end())
    {
        set<std::string> out;
        return out;
    }
    return dependents.find(s)->second;
}

void DependencyGraph::addDependency(string s, string t)
{
    std::set<int>::iterator iter;
    std::map<string,set<string> >::iterator it;
    int dependeeValue=0;
    it=dependees.find(s);
    if(it!=dependees.end())
    {
        dependees[s].insert(t);
        /*if(dependeeValue<dependee[s].size())
        {

        }*/
    }
    else
    {
        set<string> newSet;
        dependees[s]=newSet;
        dependees[s].insert(t);

    }

    it=dependents.find(s);
    if(it!=dependents.end())
    {
        dependents[t].insert(s);
    }

    else
    {
        set<string> newSet;
        dependents[t]=newSet;
        dependents[t].insert(s);
    }
}

void DependencyGraph::removeDependency(string s, string t)
{
     std::set<int>::iterator setIter;
     std::map<string,set<string> >::iterator it;

     it=dependees.find(s);

    if(it!=dependees.end() && dependees[s].find(t)!=dependees[s].end())
    {
        dependees[s].erase(t);
    }

    it=dependents.find(t);
    
    if(it!=dependents.end() && dependents[t].find(s)!=dependents[t].end())
    {
        dependents[t].erase(s);
    }
}

void DependencyGraph::replaceDependents(string s, set<string> newDependents)
{
    std::map<string,set<string> >::iterator it;
    it=dependees.find(s);
    if (it!=dependees.end())
    {
        auto copiedSet = it->second;
        for (auto str : copiedSet)
        {
          removeDependency(s, str);
        }
    }

    set<string>::iterator iter;
    for (iter = newDependents.begin(); iter != newDependents.end(); ++iter) 
    {
        addDependency(s,*iter);
    }
}

void DependencyGraph::replaceDependees(string s, set<string> newDependeees)
{
    std::map<string,set<string> >::iterator it;
    it=dependents.find(s);
    if(it!=dependents.end())
    {
      dependents[s].clear();
    }

    set<string>::iterator iter;
    for (iter = newDependeees.begin(); iter != newDependeees.end(); ++iter) 
    {
        addDependency(s,*iter);
    }
}


/**
 * Circular Dependency checking functions
 */

set<string> DependencyGraph::GetDirectDependents(string name)
{
	set<string> dependList;

	set<string>::iterator iter;

	set<string> dependents = getDependents(name);

	for (iter = dependents.begin(); iter != dependents.end(); ++iter)
	{
		dependList.insert(*iter);
	}

	return dependList;
}

void DependencyGraph::Visit(string start, string name, set<string> visited, list<string> changed)
{
    //bool CircularException=false;
	visited.insert(name);

	set<string>::iterator forIter;
	set<string>::iterator containsIter;
	set<string> dependents = GetDirectDependents(name);
	for (forIter = dependents.begin(); forIter != dependents.end(); ++forIter)
	{
		if (*forIter == start)
		{
			//CircularException=true;
            CircularException cir;
            throw cir;
            //return CircularException;
		}
		else
		{
			containsIter = visited.find(*forIter);
			if (containsIter == visited.end())
			{
				Visit(start, *forIter, visited, changed);
			}
		}
		changed.push_front(name);
	}
    //return CircularException;

}


list<string> DependencyGraph::GetCellsToRecalculate(set<string> names)
{
	list<string> changed;
	set<string> visited;

	set<string>::iterator namesIter;
	set<string>::iterator visitedIter;

	for (namesIter = names.begin(); namesIter != names.end(); ++namesIter)
	{
		visitedIter = visited.find(*namesIter);
		if (visitedIter == visited.end())
		{
			Visit(*namesIter, *namesIter, visited, changed);
		}
	}
	return changed;
}

list<string> DependencyGraph::GetCellsToRecalculate(string name)
{
  set<string> nameSet;
  nameSet.insert(name);
	return GetCellsToRecalculate(nameSet);
}

