use std::fs;

fn main() {
    let contents = fs::read_to_string("part2-input.txt").expect("Something went wrong reading the file");
    let numbers = get_numbers(contents);
    let sum: i32 = numbers.iter().sum();
    println!("Sum: {}", sum);
}

fn get_numbers(contents: String) -> Vec<i32> {
    let mut numbers: Vec<i32> = Vec::new();
    for line in contents.lines() {
        let line = string_to_digits(line);
        //println!("{}", line);
        let number = get_number(&line);
        numbers.push(number);
    }
    numbers
}

// Ugly hack that probably shouldn't be done this way, but oh well `¯\_(ツ)_/¯`
fn string_to_digits(line: &str) -> String {
    let digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    for (index, _) in line.char_indices() {
        for (digit_index, digit) in digits.iter().enumerate() {
            let number = (digit_index + 1) as i32;
            if line[index..].starts_with(digit) {
                let result = line.replacen(digit, &number.to_string(), 1);
                return string_to_digits(&result)
            }
        }
    }
    line.to_string()
}
    
fn get_number(line: &str) -> i32 {
    let first_digit = line.chars().find(|c| c.is_digit(10)).unwrap_or('0');
    let last_digit = line.chars().rev().find(|c| c.is_digit(10)).unwrap_or('0');
    let concatenated = format!("{}{}", first_digit, last_digit).parse::<i32>().unwrap_or(0);
    concatenated
}

