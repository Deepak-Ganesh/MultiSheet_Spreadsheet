#include "ErrorMessage.h"

/**
* Default construtor for JSON
*/
ErrorMessage::ErrorMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Copy Constructor. Takes in a json object of an ErrorMessage and returns an ErrorMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
ErrorMessage::ErrorMessage(json const & js)
{
    type = "error";

    code = js["code"].get<int>();
    source = js["source"].get<string>();
}

/**
* Constructor that takes in an error code and the source of the error (the cell).
*/
ErrorMessage::ErrorMessage(int _code, string _source)
{
    type = "error";

    code = _code;
    source = _source;
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json ErrorMessage::getJson()
{
    json j;
    j["type"]=type;
    j["code"]=code;
    j["source"]=source;

    return j;
}

/**
* This takes an ErrorMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string ErrorMessage::toString()
{
    json jObject = this -> getJson();
    string result = jObject.dump();
    result += "\n\n";

    return result;
}

//Getters and setters after this point, pretty self-explanitory

string ErrorMessage::getType()
{
    return type;
}

int ErrorMessage::getCode()
{
    return code;
}

string ErrorMessage::getSource()
{
    return source;
}

void ErrorMessage::setCode(int newCode)
{
    code = newCode;
}

void ErrorMessage::setSource(string newSource)
{
    source = newSource;
}
