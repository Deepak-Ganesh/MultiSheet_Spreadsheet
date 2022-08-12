#include <string>
#include <nlohmann/json.hpp>

using json = nlohmann::json;

class AdminEditOverviewMessage
{
    public:
    std::string type;
    std::string name;
    std::string owner;
    std::string edits;
    AdminEditOverviewMessage(std::string, std::string, std::string);
    json get_json();


};