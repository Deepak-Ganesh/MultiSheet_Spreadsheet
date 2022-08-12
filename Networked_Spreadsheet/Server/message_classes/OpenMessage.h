#ifndef SERVER_MESSAGE_CLASSES_OPEN_MESSAGE_H
#define SERVER_MESSAGE_CLASSES_OPEN_MESSAGE_H

#include <nlohmann/json.hpp>
#include <string>
#include <iostream>

using namespace std;
using json = nlohmann::json;

class OpenMessage
{
    private:
        string type;
        string name;
        string username;
        string password;
        json getJson(); //this is private, as the json it creates needs to pass through toString() to be formatted properly and generate string output.

    public:
        OpenMessage();
        OpenMessage(json const & js);
        OpenMessage(string _name,string _username, string _password);

        string getType();
        string getName();
        string getUsername();
        string getPassword();

        void setName(string newName);
        void setUsername(string newUsername);
        void setPassword(string newPassword);
        
        string toString();
};

#endif
