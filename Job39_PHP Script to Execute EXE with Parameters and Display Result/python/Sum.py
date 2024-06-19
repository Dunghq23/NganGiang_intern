import sys

if len(sys.argv) != 3:
    print("Usage: sum.py <num1> <num2>")
    sys.exit(1)

try:
    num1 = int(sys.argv[1])
    num2 = int(sys.argv[2])
except ValueError:
    print("Both arguments must be valid integers.")
    sys.exit(1)

sum_result = num1 + num2

with open("result.txt", "w") as f:
    f.write(str(sum_result))
