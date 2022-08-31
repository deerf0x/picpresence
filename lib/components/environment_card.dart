import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class EnvironmentCard extends StatelessWidget {
  final String name;
  final int maxCapacity;
  final int currentCapacity;

  EnvironmentCard(
      {required this.name,
      required this.maxCapacity,
      required this.currentCapacity});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(left: 25.0, bottom: 300),
      child: Container(
        padding: EdgeInsets.all(6),
        width: 200,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(16),
          color: Colors.black54
        ),
        child: Column(children: [
          ClipRRect(
              borderRadius: BorderRadius.circular(8),
              child: Image.asset('lib/images/env1.jpg')),
          Padding(
              padding: EdgeInsets.only(top: 16),
              child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(name, style: GoogleFonts.lexendDeca(fontSize: 20)),
                    SizedBox(height: 4),
                    Padding(
                        padding: EdgeInsets.only(right: 16),
                        child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text('Max Capacity',
                                  style: GoogleFonts.robotoCondensed(
                                      fontWeight: FontWeight.bold,
                                      fontSize: 14)),
                              Text(maxCapacity.toString(),
                                  style: GoogleFonts.robotoCondensed(
                                      fontSize: 14)),
                            ])),
                    Padding(
                        padding: EdgeInsets.only(right: 16),
                        child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text('Current Capacity',
                                  style: GoogleFonts.robotoCondensed(
                                      fontWeight: FontWeight.bold,
                                      fontSize: 14)),
                              Text(currentCapacity.toString(),
                                  style: GoogleFonts.robotoCondensed(
                                      fontSize: 14)),
                            ]))
                  ]))
        ]),
      ),
    );
  }
}
