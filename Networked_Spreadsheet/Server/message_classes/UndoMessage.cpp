#include "UndoMessage.h"

#include <iostream>

/**
* Default construtor for JSON, only constructor needed
*/
UndoMessage::UndoMessage()
{
    type="undo";
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json UndoMessage::getJson()
{
    json j;
    j["type"]=type;

    return j;
}

/**
* This takes an UndoMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string UndoMessage::toString()
{
    json jObject = this -> getJson();
    string result = jObject.dump();
    result += "\n\n";

    return result;
}

/**
* Getter for type
*/
string UndoMessage::getType()
{
    return type;
}