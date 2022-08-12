#include "save_server.h"

#include <nlohmann/json.hpp>

void save_server(std::string file_name, Server const & server)
{
    using namespace nlohmann;

    std::ofstream f (file_name);

    json obj;

    //obj["users"] = server.get_accounts().to_json();
    //obj["documents"] = server.get_documents().to_json;

    f << obj;
}