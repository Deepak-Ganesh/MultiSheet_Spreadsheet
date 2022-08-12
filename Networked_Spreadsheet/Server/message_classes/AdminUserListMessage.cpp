#include "AdminUserListMessage.h"

AdminUserListMessage::AdminUserListMessage(std::map<std::string, bool> users)
{
    this->users = users;
}

json AdminUserListMessage::get_json()
{
    json j;
    j["type"] = "user list";
    j["users"] = this->users;
    return j;
}
