let parse_digits = (line) => {
    let digits = line.substring(10).trim().split(/\s+/);
    return digits;
};
let parse_input = (input) => {
    let lines = input.split('\n');
    let times = parse_digits(lines[0]);
    let distances = parse_digits(lines[1]);
    let races = [];

    for (let i = 0; i < times.length; i++) {
        races.push({
            time: parseInt(times[i]),
            distance: parseInt(distances[i])
        });
    }

    return races;
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
const races = parse_input(fileContents);
let ways_product = 1;
races.forEach(race => {
    ways_product *= winning_ways(race);
});
console.log("Ways: " + ways_product);