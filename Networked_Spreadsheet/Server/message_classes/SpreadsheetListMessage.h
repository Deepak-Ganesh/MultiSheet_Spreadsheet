#ifndef SERVER_MESSAGE_CLASSES_SPREADSHEET_LIST_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_SPREADSHEET_LIST_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>
#include <vector>
#include <iostream>

using namespace std;
using json = nlohmann::json;

class SpreadsheetListMessage
{
    private:
        string type;
        vector<string> spreadsheets;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        SpreadsheetListMessage();
        SpreadsheetListMessage(json const & js);
        SpreadsheetListMessage(vector<string> _spreadsheets);
        
        string getType();
        vector<string> getSpreadsheets();

        string toString();
};

#endif
