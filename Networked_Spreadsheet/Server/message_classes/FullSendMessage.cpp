#include "FullSendMessage.h"

/**
* Default construtor for JSON
*/
FullSendMessage::FullSendMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Copy Constructor. Takes in a json object of an FullSendMessage and returns a FullSendMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
FullSendMessage::FullSendMessage(json const & js)
{
    type = "full send";

    spreadsheet = js["spreadsheet"].get<map<string,string>>();
}

/**
* Constructor that takes in a map representation of Spreadsheet _spreadsheet to
* to be sent as a message.
*/
FullSendMessage::FullSendMessage(map<string,string> _spreadsheet)
{
    type = "full send";

    spreadsheet = _spreadsheet;
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json FullSendMessage::getJson()
{
    json j;
    j["type"]=type;
    j["spreadsheet"]= spreadsheet;
    
    return j;
}

/**
* This takes an FullSendMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string FullSendMessage::toString()
{
    json jObject = this -> getJson();
    string out = jObject.dump();
    out += "\n\n";

    return out;
}

//Getters after this point. Pretty self-explanitory.

string FullSendMessage::getType()
{
    return type;
}

map<string,string> FullSendMessage::getSpreadsheet()
{
    return spreadsheet;
}