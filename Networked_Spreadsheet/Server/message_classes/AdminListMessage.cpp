#include "AdminListMessage.h"

AdminListMessage::AdminListMessage(std::vector<std::string> spreadsheets)
{
    this->spreadsheets = spreadsheets;
}

json AdminListMessage::get_json()
{
    json j;
    j["type"] = "list";
    j["spreadsheets"] = this->spreadsheets;
    return j;
}
