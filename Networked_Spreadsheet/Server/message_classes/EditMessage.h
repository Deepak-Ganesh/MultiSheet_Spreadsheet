#ifndef SERVER_MESSAGE_CLASSES_EDIT_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_EDIT_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>
#include <vector>
#include <iostream>

using namespace std;
using json = nlohmann::json;

class EditMessage
{
    private:
        string type;
        string cell;
        string value;
        vector<string> dependencies;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        EditMessage();
        EditMessage(json const & js);
        EditMessage(string _cell, string _value, vector<string> _dependencies);
        
        string getType();
        string getCell();
        string getValue();
        vector<string> getDependencies();
        
        void setCell(string newCell);
        void setValue(string newValue);
        
        string toString();
};

#endif
