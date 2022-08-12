#include "EditMessage.h"


/*int main()
        {
            vector<string> listofstuff;
            listofstuff.push_back("a6");
            listofstuff.push_back("g3");
            EditMessage test("gg","55",listofstuff);
            cout<<test.getJson().dump(4)<<endl;
        }*/

/**
* Default construtor for JSON
*/
EditMessage::EditMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Copy Constructor. Takes in a json object of an EditMessage and returns an EditMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
EditMessage::EditMessage(json const & js)
{
    type = "edit";

    cell = js["cell"].get<string>();
    value = js["value"].get<string>();
    dependencies = js["dependencies"].get<vector<string>>();
}

/**
* Constructor that takes in a cell name (to be edited), 
* the new value of the cell, and a list of dependencies for that cell.
*/
EditMessage::EditMessage(string _cell, string _value, vector<string> _dependencies)
{
    type = "edit";

    cell = _cell;
    value = _value;
    dependencies = _dependencies;
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json EditMessage::getJson()
{
    json j;
    j["type"]=type;
    j["cell"]=cell;
    j["value"]=value;
        
    j["dependencies"]= dependencies;
    
    return j;
}

/**
* This takes an EditMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string EditMessage::toString()
{
    json jObject = this -> getJson();
    string out = jObject.dump();
    out += "\n\n";

    return out;
}

//Getters and setters after this point. Pretty self-explanitory. 

string EditMessage::getType()
{
    return type;
}

string EditMessage::getCell()
{
    return cell;
}

string EditMessage::getValue()
{
    return value;
}

vector<string> EditMessage::getDependencies()
{
    return dependencies;
}

void EditMessage::setCell(string newCell)
{
    cell = newCell;
}

void EditMessage::setValue(string newValue)
{
    value = newValue;
}