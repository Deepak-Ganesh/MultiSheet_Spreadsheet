#include <string>
#include <map>
#include <nlohmann/json.hpp>

using json = nlohmann::json;

class AdminUserListMessage
{
    public:
        std::string type;
        std::map<std::string, bool> users;
        AdminUserListMessage(std::map<std::string, bool>);
        json get_json();
};