// ignore_for_file: prefer_const_constructors

import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:picpresence/components/environment_card.dart';
import 'package:picpresence/model/Environment.dart';
import 'package:pocketbase/pocketbase.dart';

class HomePage extends StatefulWidget {
  const HomePage({Key? key}) : super(key: key);

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final client = PocketBase('https://picpresence.ncastillo.xyz');
  final List<Environment> _environments = [];

  @override
  void initState() {
    super.initState();
    _fetchEnvironments();
  }

  void _fetchEnvironments() async {
    client.users.authViaEmail('arellano@pic.com', 'arellano123');
    // Subscribe to changes in any record from the collection

    // alternatively you can also fetch all records at once via getFullList:
    final records =
        await client.records.getFullList('rooms', batch: 200, sort: '-created');

    for (var element in records) {
      _environments.add(Environment.fromJson(element.toJson()));
    }

    await client.realtime.subscribe('rooms', (e) {
      setState(() {

        Environment findEnv = Environment.fromJson(e.record!.toJson());

        _environments[_environments
            .indexWhere((element) => element.name == findEnv.name)] = findEnv;
      });
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        // ignore: prefer_const_literals_to_create_immutable,
        body: Container(
      decoration: BoxDecoration(
        image: DecorationImage(
            image: AssetImage("lib/images/bgblur.png"),
            fit: BoxFit.cover,
            colorFilter: ColorFilter.mode(Colors.black54, BlendMode.darken)),
      ),
      child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 30.0, vertical: 32),
          child: Text('PICPRESENCE',
              style: GoogleFonts.lexendDeca(fontSize: 40),
              textAlign: TextAlign.start),
        ),
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 30.0),
          child: Text(
            'A PIC microcontroller presence detection system for Windows',
            style: GoogleFonts.lato(),
          ),
        ),
        SizedBox(height: 50),

        Container(
          height: 50,
          child: ListView(
            scrollDirection: Axis.horizontal,
            children: const [
              Padding(
                  padding: EdgeInsets.only(left: 25),
                  child: Text('Environments',
                      style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: Colors.orange))),
              Padding(
                  padding: EdgeInsets.only(left: 25),
                  child: Text('History',
                      style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: Colors.orange))),
            ],
          ),
        ),

        Expanded(
            child: ListView(
          scrollDirection: Axis.horizontal,
          children: <Widget>[
            for (var env in _environments)
              EnvironmentCard(
                name: env.name,
                currentCapacity: env.currentCapacity,
                maxCapacity: env.maxCapacity,
              )
          ],
        )),
        // SizedBox(height: 150),
      ]),
    ));
  }
}
