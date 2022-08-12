/** Contains the class declaration of Server.
 *
 *  16 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_USER_ACCOUNT_COLLECTION_H
#define SERVER_USER_ACCOUNT_COLLECTION_H

#include <map>
#include <thread>
#include <asio.hpp>
#include <nlohmann/json.hpp>

#include "Document.h"

/** Contains all data about the server's user.
 * 
 * It's essentially just a map from username to password (both std::string).
 */
class UserAccountCollection
{
  public:
    /** Adds the account if it doesn't exist, returning true.
     * 
     * @param username The username of the account.
     * @param password The password of the account.
     * 
     * @return True if the username didn't previously exist.
     */
    bool add_account(std::string username, std::string password);

    /** Remove the account if it exists, returning true.
     * 
     * @param username The username of the account.
     * 
     * @return True if the username previously existed.
     */
    bool delete_account(std::string username);

     /** Change the password if account exists, returning true.
     * 
     * @param username The username of the account.
     * @param password The password of the account.
     * 
     * @return True if the username exists.
     */
    bool change_password(std::string username, std::string password);

    /** Adds the account if it doesn't exist, or checks the password
     * if it does exist.
     * 
     * @param username The username of the account to authenticate.
     * @param password The password of the account to authenticate.
     * 
     * @return True if the username exists and the password matches
     */
    bool authenticate(std::string username, std::string password);


    /** Authenticates the user if it exists, otherwise creates it.
     * 
     * @param username The username of the account.
     * @param password The passwor of the account.
     * 
     * @return True if the username and password match, or if the user didn't
     *         previously exist. False if the username and password don't match.
     */
    bool attempt_authentication(std::string username, std::string password);


    /** Creates a json object from this object's state.
     * The json object is a dictionary of usernames mapped to passwords.
     * 
     * @return The json object.
     */
    nlohmann::json serialize();

    nlohmann::json serialize_names();

    //Returns a vector containing all the usernames in the collection
    std::vector<std::string> get_names();


    /** Sets the internal data to the json-serialized data.
     * The json object must be a dictionary of usernames mapped to passwords.
     * 
     * @param js The json object to deserialize.
     */
    void deserialize(nlohmann::json & js);


  private:
    /** Used for thread locking. */
    std::mutex mut;

    /** Contains the user account data. */
    std::map<std::string, std::string> nameToPasswordMap;
};

#endif //SERVER_USER_ACCOUNT_COLLECTION_H
