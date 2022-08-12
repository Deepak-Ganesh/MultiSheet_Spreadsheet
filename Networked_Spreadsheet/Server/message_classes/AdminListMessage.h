
#include <string>
#include <vector>
#include <nlohmann/json.hpp>

using json = nlohmann::json;

class AdminListMessage
{
    public:
        std::string type;
        std::vector<std::string> spreadsheets;
        AdminListMessage(std::vector<std::string> spreadsheets);
        json get_json();

};