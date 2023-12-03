use std::fs;
use std::env;

fn main() {
    let args: Vec<String> = env::args().collect();
    if args.len() < 2 {
        println!("Please provide a file path as a command line argument");
        return;
    }
    let file_path = &args[1];

    let contents = fs::read_to_string(file_path).expect("Something went wrong reading the file");
    let numbers = get_part_numbers(contents);
    let sum: i32 = numbers.iter().sum();
    println!("Sum: {}", sum);
}

fn get_part_numbers(contents: String) -> Vec<i32> {
    let symbols = get_symbol_coords(&contents);
    let mut numbers: Vec<i32> = Vec::new();
    for symbol in symbols {
        let adjacent = get_adjacent_numbers(symbol, &contents);
        for num in adjacent {
            if !numbers.contains(&num) {
                numbers.push(num);
            }
        }
    }
    numbers
}

fn get_symbol_coords(contents: &String) -> Vec<(i32, i32)> {
    let mut symbols: Vec<(i32, i32)> = Vec::new();
    for (line_num, line) in contents.lines().enumerate() {
        for (i, c) in line.chars().enumerate() {
            if c != '.' && !c.is_digit(10) {
                symbols.push((i as i32, line_num as i32));
            }
        }
    }
    symbols
}

fn get_adjacent_numbers(symbol: (i32, i32), contents: &String) -> Vec<i32> {
    let mut numbers: Vec<i32> = Vec::new();
    let (x, y) = symbol;
    if x > 0 { // left
        if y > 0 { // top
            println!("Checking top left from ({}, {})", x, y);
            let top_left = get_char_at(&contents, x-1, y-1);
            if top_left.is_digit(10) {
                numbers.push(get_full_number(&contents, x - 1, y - 1));
            }
        }
        println!("Checking left from ({}, {})", x, y);
        let left = get_char_at(&contents, x-1, y);
        if left.is_digit(10) {
            numbers.push(get_full_number(&contents, x - 1, y));
        }
        if y < contents.lines().count() as i32 - 1 { // bottom
            println!("Checking bottom left from ({}, {})", x, y);
            let bottom_left = get_char_at(&contents, x-1, y+1);
            if bottom_left.is_digit(10) {
                numbers.push(get_full_number(&contents, x - 1, y + 1));
            }
        }
    }
    if y > 0 { // top
        println!("Checking top from ({}, {})", x, y);
        let top = get_char_at(&contents, x, y-1);
        if top.is_digit(10) {
            numbers.push(get_full_number(&contents, x, y - 1));
        }
    }
    if y < contents.lines().count() as i32 - 1 { // bottom
        println!("Checking bottom from ({}, {})", x, y);
        let bottom = get_char_at(&contents, x, y+1);
        if bottom.is_digit(10) {
            numbers.push(get_full_number(&contents, x, y + 1));
        }
    }
    numbers
}

fn get_char_at(contents: &String, x: i32, y: i32) -> char {
    if x < 0 || y < 0 || y >= contents.lines().count() as i32 || x >= contents.lines().nth(y as usize).unwrap().chars().count() as i32 {
        return '.';
    }
    contents.lines().nth(y as usize).unwrap().chars().nth(x as usize).unwrap()
}

fn get_full_number(contents: &String, x: i32, y: i32) -> i32 {
    let mut number = String::new();
    let mut x = x;
    loop {
        // if x < 0 {
        //     break;
        // }
        if x > 0 && get_char_at(&contents, x-1, y).is_digit(10) {
            x -= 1;
        } else {
            break;
        }
    }
    println!("Found start of number at: {}, {}", x, y);
    loop {
        let c = get_char_at(&contents, x, y);
        if c.is_digit(10) {
            number.push(c);
            x += 1;
        } else {
            break;
        }
    }
    println!("Found number: {}", number);
    number.parse::<i32>().unwrap()
}