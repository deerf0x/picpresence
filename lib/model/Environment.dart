
class Environment {

  final String name;
  final int maxCapacity;
  final int currentCapacity;

  Environment({ required this.name, required this.maxCapacity, required this.currentCapacity});

  factory Environment.fromJson(Map<String, dynamic> json) {
    return Environment(
      name: json['name'],
      maxCapacity: json['maxCapacity'],
      currentCapacity: json['currentCapacity'],
    );
  }

}