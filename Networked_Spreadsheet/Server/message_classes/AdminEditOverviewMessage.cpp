#include "AdminEditOverviewMessage.h"

AdminEditOverviewMessage::AdminEditOverviewMessage(
    std::string name, std::string owner, std::string edits)
{
    this->name = name;
    this->owner = owner;
    this->edits = edits;
}

json AdminEditOverviewMessage::get_json()
{
    json j;
    j["type"] = "edit overview";
    j["name"] = this->name;
    j["owner"] = this->owner;
    j["edits"] = this->edits;
    return j;
}
