#ifndef SERVER_MESSAGE_CLASSES_ERROR_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_ERROR_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>
#include <iostream>

using namespace std;
using json = nlohmann::json;

class ErrorMessage
{
    private:
        string type;
        int code;
        string source;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        ErrorMessage();
	ErrorMessage(json const & js);
        ErrorMessage(int _code, string _source);

        string getType();
        int getCode();
        string getSource();

        void setCode(int newCode);
        void setSource(string newSource);
        
	string toString();
};

#endif
