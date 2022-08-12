#include "OpenMessage.h"

/*
int main()
        {
            
            OpenMessage test("deepak","ganesh","lastname");
            cout<<test.getJson().dump(4)<<endl;
        }*/

/**
* Default construtor for JSON
*/
OpenMessage::OpenMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Copy Constructor. Takes in a json object of an OpenMessage and returns an OpenMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
OpenMessage::OpenMessage(json const & js)
{
    type = "open";

    name = js["name"].get<string>();
    username = js["username"].get<string>();
    password = js["password"].get<string>();
}

/**
* Constructor that takes in a spreadsheet name, 
* the username of the requesting user, and the password of the requesting user.
*/
OpenMessage::OpenMessage(string _name,string _username, string _password)
{
    type = "open";

    name = _name;
    username = _username;
    password = _password;
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json OpenMessage::getJson()
{
    json j;
    j["type"]=type;
    j["name"]=name;
    j["username"]=username;
    j["password"]= password;

    return j;
}

/**
* This takes an OpenMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string OpenMessage::toString()
{
    json jObject = this -> getJson();
    string result = jObject.dump();
    result += "\n\n";

    return result;
}

//Getters and setters after this point.

string OpenMessage::getType()
{
    return type;
}

string OpenMessage::getName()
{
    return name;
}

string OpenMessage::getUsername()
{
    return username;
}

string OpenMessage::getPassword()
{
    return password;
}

void OpenMessage::setName(string newName)
{
    name = newName;
}

void OpenMessage::setUsername(string newUsername)
{
    username = newUsername;
}

void OpenMessage::setPassword(string newPassword)
{
    password = newPassword;
}
