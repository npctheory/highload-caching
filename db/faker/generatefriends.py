import random

def extract_user_ids(filename):
    """Extract user IDs from an SQL file."""
    user_ids = []
    with open(filename, 'r') as f:
        for line in f:
            if line.startswith("INSERT INTO users"):
                # Extract the user_id from the VALUES clause
                parts = line.split("VALUES (")[1]
                user_id = parts.split(",")[0].strip("('")
                user_ids.append(user_id)
    return user_ids

def generate_friends_sql(user_ids, num_friends, output_filename, batch_size=5000):
    """Generate SQL insert statements for friendships in bulk."""
    with open(output_filename, 'w') as f:
        # Write the initial database connection line
        f.write("\\c highload_social;\n\n")

        existing_relationships = set()
        insert_statements = []

        for user_id in user_ids:
            # Potential friends excluding the user themselves
            potential_friends = list(set(user_ids) - {user_id})
            # Select a random sample of friends
            friends = random.sample(potential_friends, min(num_friends, len(potential_friends)))

            for friend_id in friends:
                # Ensure unique bidirectional relationships
                if (user_id, friend_id) not in existing_relationships and (friend_id, user_id) not in existing_relationships:
                    insert_statements.append(f"('{user_id}', '{friend_id}')")
                    insert_statements.append(f"('{friend_id}', '{user_id}')")
                    existing_relationships.add((user_id, friend_id))
                    existing_relationships.add((friend_id, user_id))

                    # Write in bulk if batch size is reached
                    if len(insert_statements) >= batch_size:
                        f.write("INSERT INTO friends (id, user_id) VALUES\n")
                        f.write(",\n".join(insert_statements) + ";\n")
                        insert_statements = []

        # Write remaining statements if any
        if insert_statements:
            f.write("INSERT INTO friends (id, user_id) VALUES\n")
            f.write(",\n".join(insert_statements) + ";\n")

if __name__ == "__main__":
    user_ids = extract_user_ids('db/initdb/init001users.sql')
    generate_friends_sql(user_ids, 20, 'db/initdb/init003friends.sql')
