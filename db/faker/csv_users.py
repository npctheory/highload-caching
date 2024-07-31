import csv
import hashlib
import os
import random
from faker import Faker

# Initialize the Faker library
fake = Faker()

# Define the number of users to generate
num_users = 5000

# Define the CSV file path
csv_directory = './db/csv/'
csv_file = os.path.join(csv_directory, 'fake_users.csv')

# Ensure the directory exists
os.makedirs(csv_directory, exist_ok=True)

# Function to generate a hashed password
def generate_password_hash(password):
    return hashlib.sha256(password.encode()).hexdigest()

# Function to generate a human-readable ID
def generate_human_readable_id():
    word1 = fake.word().capitalize()
    word2 = fake.word().capitalize()
    number = random.randint(1000, 9999)
    return f"{word1}{word2}{number}"

# Generate fake user data and write to CSV
with open(csv_file, mode='w', newline='') as file:
    writer = csv.writer(file)
    # Write the header
    writer.writerow(['id', 'password_hash', 'first_name', 'second_name', 'birthdate', 'biography', 'city'])
    
    for _ in range(num_users):
        user_id = generate_human_readable_id()
        password_hash = generate_password_hash(fake.password())
        first_name = fake.first_name()
        second_name = fake.last_name()
        birthdate = fake.date_of_birth(minimum_age=18, maximum_age=90).isoformat()
        biography = fake.word()
        city = fake.city()
        
        # Write the user data to the CSV file
        writer.writerow([user_id, password_hash, first_name, second_name, birthdate, biography, city])

print(f'{num_users} fake users have been written to {csv_file}')
