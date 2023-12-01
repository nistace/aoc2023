﻿#pragma once
#include <string>

#include "../../Utils/input_reader.h"

static std::string part1()
{
    constexpr char digits_as_numbers[10] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

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
    return "blob";
}
