CXXFLAGS = -std=c++11 -Wall -Wextra -g -Wno-unused-parameter
LDFLAGS = -L/opt/ros/groovy/lib -lroscpp -lroscpp_serialization -lcpp_common -lrosconsole -lrostime

OUTPUTS = image

default: all
all: $(OUTPUTS)
rebuild: clean all

%: %.cpp
	$(CXX) $(CXXFLAGS) -o $@ $^ $(LDFLAGS)

clean:
	rm -f $(OUTPUTS)

