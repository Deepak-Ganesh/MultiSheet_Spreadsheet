#ifndef SERVER_MESSAGE_CLASSES_REVERT_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_REVERT_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>

using namespace std;
using json = nlohmann::json;

class RevertMessage
{
    private:
        string type;
        string cell;   
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        RevertMessage();
        RevertMessage(json const & js);
        RevertMessage(string _cell);
        
        string getType();
        string getCell();
        void setCell(string newCell);
        
        string toString();        
};

#endif
