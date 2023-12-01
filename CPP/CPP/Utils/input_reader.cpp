#include "input_reader.h"
#include <fstream>
#include <iostream>
#include <string>

std::list<std::string> input_reader::read_file(const int day)
{
    std::list<std::string> lines;
    std::string day_as_string = std::to_string(day);
    if (day_as_string.size() == 1) day_as_string = "0" + day_as_string;
    std::ifstream file("Inputs/day_" + day_as_string + ".txt");
    if (file.is_open())
    {
        std::string next_line;
        while (std::getline(file, next_line))
        {
            lines.push_back(next_line);
        }
    }
    else
    {
        std::cerr << "file no open";
    }
    return lines;
}
