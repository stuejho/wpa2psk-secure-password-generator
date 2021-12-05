import csv

# 1 million to 100 trillion
BASE_SPEED_PER_SECOND = 1_000_000 # 1 million
MAX_SPEED_PER_SECOND = 100_000_000_000_000 # 100 trillion

seconds_per_year = 60 * 60 * 24 * 365
passwords = 62 ** 63

header = ['Length', '1 million', '10 million', '100 million', '1 billion', '10 billion', '100 billion', '1 trillion', '10 trillion', '100 trillion']

# Write CSV of calculations
with open('output.csv', 'w', newline='', encoding='utf-8') as f:
    writer = csv.writer(f)
    
    # Write header row
    writer.writerow(header)
    
    # Lengths from 1 to 63
    for i in range(1, 64):
        # Number of password combinations
        passwords = 62 ** i
        
        # Reset speed and row data
        speed_per_second = BASE_SPEED_PER_SECOND
        row_data = []
        row_data.append(i)
        
        # Calculate cracking time for different speeds
        while speed_per_second <= MAX_SPEED_PER_SECOND:
            seconds = passwords / speed_per_second
            years = seconds / seconds_per_year
            years_fmt = '{:.2e}'.format(years)
            row_data.append(years_fmt)
            
            # Increment speed by a factor of 10
            speed_per_second *= 10
        
        # Write out row data
        writer.writerow(row_data)