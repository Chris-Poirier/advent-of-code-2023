use std::fs;

fn main() {
    let contents = fs::read_to_string("part1-input.txt").expect("Something went wrong reading the file");
    let numbers = get_numbers(contents);
    let sum: i32 = numbers.iter().sum();
    println!("Sum: {}", sum);
}

fn get_numbers(contents: String) -> Vec<i32> {
    let mut numbers: Vec<i32> = Vec::new();
    for line in contents.lines() {
        let number = get_number(line);
        numbers.push(number);
    }
    numbers
}

fn get_number(line: &str) -> i32 {
    let first_digit = line.chars().find(|c| c.is_digit(10)).unwrap_or('0');
    let last_digit = line.chars().rev().find(|c| c.is_digit(10)).unwrap_or('0');
    let concatenated = format!("{}{}", first_digit, last_digit).parse::<i32>().unwrap_or(0);
    concatenated
}

