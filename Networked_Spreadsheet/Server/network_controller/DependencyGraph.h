#ifndef Dependency_Graph_h
#define Dependency_Graph_h
#include <string>
#include <set>
#include <list>
#include <map>

using namespace std;

class DependencyGraph{
    private:
        //the dependents map the keys are the dependents and the values are the dependees
        map<string,set<string> > dependents;
        //in the dependees map the keys are the dependees and the values are the dependents
        map<string,set<string> > dependees;

    public:

    //Dependency Graph
    std::set<std::string> getDependents(std::string s);
    std::set<std::string> getDependees(std::string s);
    void addDependency(std::string s,std::string t);
    void removeDependency(std::string s, std::string t);
    void replaceDependents(std::string s, std::set<std::string> newDependents);
    void replaceDependees(std::string s, std::set<std::string> newDependeees);
    
    //Circular dependency checking
    std::list<std::string> GetCellsToRecalculate(std::set<std::string> names);
    std::list<std::string> GetCellsToRecalculate(std::string name);
    //bool Vist(std::string start, std::string name, std::set<std::string> visited, std::list<std::string> changed);

    set<string> GetDirectDependents(string name);
    void Visit(string start, string name, set<string> visited, list<string> changed);
};

class CircularException: public exception
{
    public:
    virtual const char* what() const throw()
    {
        return "Cirucular Exception error has occured"; 
    }
};

#endif
