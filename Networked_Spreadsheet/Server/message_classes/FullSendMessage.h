#ifndef SERVER_MESSAGE_CLASSES_FULL_SEND_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_FULL_SEND_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>
#include <map>
#include <iostream>

using namespace std;
using json = nlohmann::json;

class FullSendMessage
{
    private:
        string type;
        map<string,string> spreadsheet;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        FullSendMessage();
        FullSendMessage(json const & js);
        FullSendMessage(map<string,string> _spreadsheet);
        
        string getType();
        map<string,string> getSpreadsheet();

        string toString();
};

#endif
