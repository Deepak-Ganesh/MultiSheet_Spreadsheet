#include "RevertMessage.h"

#include <iostream>

// int main()
//         {
            
//             RevertMessage test("gg");
//             cout<<test.getJson().dump(4)<<endl;
//         }

/**
* Default construtor for JSON
*/
RevertMessage::RevertMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Constructor that takes in a cell name (to be reverted).
*/
RevertMessage::RevertMessage(string _cell)
{
    type = "revert";
    cell = _cell;
}

/**
* Copy Constructor. Takes in a json object of an RevertMessage and returns an RevertMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
RevertMessage::RevertMessage(json const & js)
{
    type = "revert";
    cell = js["cell"].get<string>();
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json RevertMessage::getJson()
{
    json j;
    j["type"]=type;
    j["cell"]=cell;

    return j;
}

/**
* This takes an RevertMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string RevertMessage::toString()
{
    json jObject = this -> getJson();
    string result = jObject.dump();
    result += "\n\n";

    return result;
}

//Getters and setters after this.

string RevertMessage::getType()
{
    return type;
}

string RevertMessage::getCell()
{
    return cell;
}

void RevertMessage::setCell(string newCell)
{
    cell = newCell;
}
