let parse_digits = (line) => {
    let digits = line.substring(10).trim().replace(/\s+/g,'');
    return digits;
};
let parse_input = (input) => {
    let lines = input.split('\n');
    let time = parse_digits(lines[0]);
    let distance = parse_digits(lines[1]);

    return {
        time: parseInt(time),
        distance: parseInt(distance)
    };
};

let winning_ways = (race) => {
    let ways = 0;
    for(let i=1; i<race.time; i++) {
        let dist = i * (race.time-i);
        if(dist > race.distance) {
            ways++;
        }
    }
    return ways;
};

const fs = require('fs');
const filename = process.argv[2];
const fileContents = fs.readFileSync(filename, 'utf-8');
const race = parse_input(fileContents);
// console.log(race);
let ways = winning_ways(race);
console.log("Ways: " + ways);