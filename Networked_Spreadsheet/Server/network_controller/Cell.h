#ifndef SERVER_NETWORK_CONTROLLER_CELL_H
#define SERVER_NETWORK_CONTROLLER_CELL_H

#include <nlohmann/json.hpp>

#include <set>

using namespace nlohmann;
using namespace std;

/** Simple type that represents a cell and its dependencies.
 */
class Cell
{
  public:
    /** Default constructor */
    Cell();

    /** Direct value constructor */
    Cell(string value, vector<string> dependencies);

    /** JSON deserialize constructor */
    Cell(json const & js);


    string get_value() const;
    vector<string> & getDependencies();
    set<string> getDependencySet() const;


    /** Serializes the cell as a JSON object.
     * 
     * @return A json object of the form {"value": ..., "dependencies": [...]}
     */
    json serialize() const;

    /** Adds fields to the json object.
     * 
     * This is useful to EditMessage because the cell value and dependencies
     * are not in an object of their own.
     * 
     * @param js The json object to add the cell's value and dependencies fields to.
     */
    void serialize_into(json & js) const;


  private:
    /** The value of the cell as a json object.
     * 
     * Can only be a string or a number.
     */
    string value;

    /** The names of the cells this cell depends on.
     * 
     * This information is taken from the edit message type.
     */
    vector<string> dependencies;
};

#endif