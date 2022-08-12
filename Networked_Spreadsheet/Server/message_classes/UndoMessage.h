#ifndef SERVER_MESSAGE_CLASSES_UNDO_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_UNDO_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>

using namespace std;
using json = nlohmann::json;

class UndoMessage
{
    private:
        string type;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        UndoMessage();
        
        string getType();
        
        string toString();
};

#endif
