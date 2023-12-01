#pragma once
#include <string>

#include "../../Utils/input_reader.h"

constexpr char digits_as_numbers[10] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
std::string digits_as_strings[10] = {
    "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
};

static std::string part1()
{
    const std::list<std::string> input_lines = input_reader::read_file(1);
    unsigned int sum = 0;
    for (const auto& input_line : input_lines)
    {
        bool first_digit_found = false;
        unsigned int last_digit = 0;
        for (const char& line_character : input_line)
        {
            for (unsigned int i = 0; i < sizeof digits_as_numbers; ++i)
            {
                if (line_character == digits_as_numbers[i])
                {
                    if (!first_digit_found)
                    {
                        first_digit_found = true;
                        sum += i * 10;
                    }
                    last_digit = i;
                }
            }
        }
        sum += last_digit;
    }
    return std::to_string(sum);
}

static std::string part2()
{
    const std::list<std::string> input_lines = input_reader::read_file(1);
    unsigned int sum = 0;
    for (const auto& input_line : input_lines)
    {
        bool first_digit_found = false;
        unsigned int last_digit = 0;
        for (unsigned int line_character_index = 0; line_character_index < input_line.size(); ++line_character_index)
        {
            const auto line_character = input_line[line_character_index];
            auto digit_found = false;
            for (unsigned int i = 0; !digit_found && i < sizeof digits_as_numbers; ++i)
            {
                if (line_character == digits_as_numbers[i])
                {
                    if (!first_digit_found)
                    {
                        first_digit_found = true;
                        sum += i * 10;
                    }
                    last_digit = i;
                    digit_found =true;
                }
            }
            for (unsigned int i = 0; !digit_found && i < 10; ++i)
            {
                auto digit_as_string = digits_as_strings[i];
                auto is_digit = true;
                for (unsigned int  j = 0; !digit_found && is_digit && j < sizeof input_line.size() && j < digit_as_string.size(); ++j)
                {
                    if (input_line[line_character_index + j] != digit_as_string[j])
                    {
                        is_digit = false;
                    }
                }
                if (is_digit)
                {
                    if (!first_digit_found)
                    {
                        first_digit_found = true;
                        sum += i * 10;
                    }
                    last_digit = i;
                    digit_found = true;
                }
            }
        }
        sum += last_digit;
    }
    return std::to_string(sum);
}
