/** Contains the class declaration of DocumentCollection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_DOCUMENT_COLLECTION_H
#define SERVER_DOCUMENT_COLLECTION_H

#include <map>
#include <thread>
#include <nlohmann/json.hpp>

#include "Document.h"

/** Contains a map of document names and Document objects.
 */
class DocumentCollection
{
  public:
    /** Either returns an already-created Document or a newly-created Document.
     *  Is thread-safe.
     */
    Document & get_or_create_document(std::string name);

    /** Remove the document if it exists, returning true.
     * 
     * @param name The name of the spreadsheet.
     * 
     * @return True if the document previously existed.
     */
    bool delete_document(std::string name);

    /** Generates a JSON string "Spreadsheet List" message.
     */
    std::string gen_list();


    /** Creates a json object from this object's state.
     * The json object is a dictionary of document names mapped to documents.
     * 
     * @return The json object.
     */
    nlohmann::json serialize();

    /** Sets the internal data to the json-serialized data.
     * The json object must be a dictionary of document names mapped to documents.
     * 
     * @param js The json object to deserialize.
     */
    void deserialize(nlohmann::json & js);

    json serialize_names();

    std::vector<std::string> get_names();

    void remove_all_connections();

  private:
    std::mutex mut;
    std::map<std::string, Document> document_map;
};

#endif //SERVER_DOCUMENT_COLLECTION_H
