all: main.o
	g++ *.o -pthread -o server.out -std=c++11  -Ivendor/asio-1.12.2/include -Ivendor/json-develop/include -DASIO_STANDALONE

main.o: main.cpp network_controller/*.cpp message_classes/*.cpp
	g++ -c -g -std=c++11 main.cpp network_controller/*.cpp message_classes/*.cpp -Ivendor/asio-1.12.2/include -Ivendor/json-develop/include -I.

clean:
	rm -f server.out *.o
