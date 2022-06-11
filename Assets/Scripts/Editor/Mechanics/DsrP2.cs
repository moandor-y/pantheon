﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Pantheon.XivSimParser;

namespace Pantheon.Mechanics {
  public static class DsrP2 {
    private const string _mechanicVisible = "MechanicVisible";
    private const string _mechanicNonTargeted = "MechanicNonTargeted";

    private const float _arenaRadius = 46.83f / 2;

    private const string _spawnThordan = "SpawnThordan";
    private const string _thordanMechanics = "ThordanMechanics";

    private const string _ascalonsMercyConcealed = "AscalonsMercyConcealed";
    private const string _ascalonsMercyConcealedTargeted = "AscalonsMercyConcealedTargeted";
    private const float _ascalonsMercyConcealedCastDuration = 2.6833333333333333333333333333333f;
    private const float _ascalonsMercyConcealedDamageDelay = 1.8833333333333333333333333333333f;

    private const string _strengthOfTheWard = "StrengthOfTheWard";
    private const float _strengthOfTheWardCastDuration = 3.6833333333333333333333333333333f;

    private const string _strengthOfTheWardHeavyImpact = "StrengthOfTheWardHeavyImpact";
    private const string _strengthOfTheWardHeavyImpactDamage = "StrengthOfTheWardHeavyImpactDamage";
    private const string _strengthOfTheWardHeavyImpactPool = "StrengthOfTheWardHeavyImpactPool";
    private const float _strengthOfTheWardHeavyImpactRadius = _arenaRadius / 6.5f;
    private const float _strengthOfTheWardHeavyImpactPosition =
        _strengthOfTheWardHeavyImpactRadius * 1.1f;
    private const float _strengthOfTheWardHeavyImpactBeforeStart =
        6.9666666666666666666666666666667f;
    private const float _strengthOfTheWardHeavyImpactCastDuration = 5.7f;
    private const float _strengthOfTheWardHeavyImpactDamageInterval =
        1.8166666666666666666666666666667f;

    private const string _strengthOfTheWardSpiralThrust = "StrengthOfTheWardSpiralThrust";
    private const string _strengthOfTheWardSpiralThrustPool = "StrengthOfTheWardSpiralThrustPool";

    public static MechanicData GetMechanicData() {
      MechanicData mechanicData = new MechanicData();
      mechanicData.referenceMechanicProperties = new Dictionary<string, MechanicProperties>();

      mechanicData.referenceMechanicProperties[_mechanicVisible] = new MechanicProperties() {
        visible = true,
      };
      mechanicData.referenceMechanicProperties[_mechanicNonTargeted] = new MechanicProperties() {
        isTargeted = false,
      };

      mechanicData.referenceMechanicProperties[_spawnThordan] = new MechanicProperties() {
        visible = false,
        mechanic =
            new SpawnEnemy() {
              enemyName = "King Thordan",
              textureFilePath = "Mechanics/Resources/Thordan.png",
              colorHtml = "#000000",
              maxHp = 1000000,
              baseMoveSpeed = 2,
              hitboxSize = 3,
              isTargetable = true,
              visualPosition = new Vector3(0, 2, 0),
              visualScale = new Vector3(4, 4, 4),
              referenceMechanicName = _thordanMechanics,
              position = new Vector2(0, 0),
            },
      };
      mechanicData.referenceMechanicProperties[_thordanMechanics] = new MechanicProperties() {
        mechanic =
            new ExecuteMultipleEvents() {
              events =
                  new List<MechanicEvent>() {
                    // new WaitEvent() {
                    //   timeToWait = 8.4333333333333333333333333333333f,
                    // },
                    // new SpawnMechanicEvent() {
                    //   referenceMechanicName = _ascalonsMercyConcealed,
                    // },
                    // new WaitEvent() {
                    //   timeToWait = 16.5f,
                    // },
                    new SpawnMechanicEvent() {
                      referenceMechanicName = _strengthOfTheWard,
                    },
                    new WaitEvent() {
                      timeToWait = float.PositiveInfinity,
                    },
                  },
            },
      };

      mechanicData.referenceMechanicProperties[_ascalonsMercyConcealed] = new MechanicProperties() {
        mechanic =
            new ExecuteMultipleEvents() {
              events =
                  new List<MechanicEvent>() {
                    new StartCastBar() {
                      castName = "Ascalon's Mercy Concealed",
                      duration = _ascalonsMercyConcealedCastDuration,
                    },
                    new SpawnTargetedEvents() {
                      referenceMechanicName = _ascalonsMercyConcealedTargeted,
                      targetingScheme = new TargetAllPlayers(),
                    },
                  },
            },
      };
      mechanicData.referenceMechanicProperties[_ascalonsMercyConcealedTargeted] =
          new MechanicProperties() {
            visible = false,
            isTargeted = true,
            followSpeed = 0,
            collisionShape = CollisionShape.Round,
            collisionShapeParams = new Vector4(100, 24, 0, 0),
            colorHtml = "#ff0000",
            mechanic =
                new ExecuteMultipleEvents() {
                  events =
                      new List<MechanicEvent>() {
                        new WaitEvent() {
                          timeToWait = _ascalonsMercyConcealedCastDuration,
                        },
                        new ModifyMechanicEvent() {
                          referenceMechanicName = _mechanicNonTargeted,
                        },
                        new WaitEvent() {
                          timeToWait = _ascalonsMercyConcealedDamageDelay,
                        },
                        new ModifyMechanicEvent() {
                          referenceMechanicName = _mechanicVisible,
                        },
                        new ApplyEffectToPlayers() {
                          effect =
                              new DamageEffect() {
                                damageAmount = 100000,
                              },
                        },
                        new WaitEvent() {
                          timeToWait = 0.8f,
                        },
                      },
                },
          };

      mechanicData.referenceMechanicProperties[_strengthOfTheWard] = new MechanicProperties() {
        visible = false,
        mechanic =
            new ExecuteMultipleEvents() {
              events =
                  new List<MechanicEvent>() {
                    new StartCastBar() {
                      castName = "Strength of the Ward",
                      duration = _strengthOfTheWardCastDuration,
                    },
                    // new WaitEvent() {
                    //   timeToWait =
                    //       _strengthOfTheWardCastDuration +
                    //       _strengthOfTheWardHeavyImpactBeforeStart,
                    // },
                    // new ExecuteRandomEvents() {
                    //   mechanicPoolName = _strengthOfTheWardHeavyImpactPool,
                    // },
                    new ExecuteRandomEvents() {
                      mechanicPoolName = _strengthOfTheWardSpiralThrustPool,
                      numberToSpawn = 3,
                    },
                  },
            },
      };
      List<string> strengthOfTheWardHeavyImpactDamageNames = new List<string>();
      for (int i = 0; i < 5; ++i) {
        strengthOfTheWardHeavyImpactDamageNames.Add($"{_strengthOfTheWardHeavyImpactDamage}{i}");
      }
      mechanicData.referenceMechanicProperties[_strengthOfTheWardHeavyImpact] =
          new MechanicProperties() {
            visible = true,
            collisionShape = CollisionShape.Round,
            collisionShapeParams = new Vector4(_strengthOfTheWardHeavyImpactRadius, 360, 0, 0),
            mechanic =
                new ExecuteMultipleEvents() {
                  events =
                      new List<MechanicEvent>() {
                        new WaitEvent() {
                          timeToWait = _strengthOfTheWardCastDuration,
                        },
                        new SpawnMechanicEvent() {
                          referenceMechanicName = strengthOfTheWardHeavyImpactDamageNames[0],
                          isPositionRelative = true,
                        },
                      },
                },
          };
      for (int i = 0; i < strengthOfTheWardHeavyImpactDamageNames.Count; ++i) {
        List<MechanicEvent> events = new List<MechanicEvent>() {
          new ApplyEffectToPlayers() {
            effect =
                new DamageEffect() {
                  damageAmount = 100000,
                },
          },
          new WaitEvent() {
            timeToWait = _strengthOfTheWardHeavyImpactDamageInterval,
          },
        };
        if (i + 1 < strengthOfTheWardHeavyImpactDamageNames.Count) {
          events.Add(new SpawnMechanicEvent() {
            referenceMechanicName = strengthOfTheWardHeavyImpactDamageNames[i + 1],
            isPositionRelative = true,
          });
        }
        mechanicData.referenceMechanicProperties[strengthOfTheWardHeavyImpactDamageNames[i]] =
            new MechanicProperties() {
              visible = true,
              collisionShape = CollisionShape.Round,
              collisionShapeParams = new Vector4(_strengthOfTheWardHeavyImpactRadius * (i + 1), 360,
                                                 _strengthOfTheWardHeavyImpactRadius * i),
              mechanic =
                  new ExecuteMultipleEvents() {
                    events = events,
                  },
            };
      }

      mechanicData.referenceMechanicProperties[_strengthOfTheWardSpiralThrust] =
          new MechanicProperties() {
            visible = true,
            collisionShape = CollisionShape.Rectangle,
            collisionShapeParams = new Vector4(
                2 * _arenaRadius, 2 * _arenaRadius * Mathf.Sin(45.0f / 2 * Mathf.Deg2Rad), 0, 0),
            mechanic =
                new ExecuteMultipleEvents() {
                  events =
                      new List<MechanicEvent>() {
                        new WaitEvent() {
                          timeToWait = float.PositiveInfinity,
                        },
                      },
                },
          };

      mechanicData.mechanicEvents = new List<MechanicEvent>() {
        new SpawnVisualObject() {
          textureFilePath = "Mechanics/Resources/ArenaCircle.png",
          relativePosition = new Vector3(0, -0.001f, 0),
          eulerAngles = new Vector3(90, 0, 0),
          scale = new Vector3(_arenaRadius * 2, _arenaRadius * 2, 1),
          visualDuration = float.PositiveInfinity,
        },
        new SpawnMechanicEvent() {
          referenceMechanicName = _spawnThordan,
        },
      };

      mechanicData.mechanicPools = new Dictionary<string, List<MechanicEvent>> {
        { _strengthOfTheWardHeavyImpactPool, new List<MechanicEvent>() },
        { _strengthOfTheWardSpiralThrustPool, new List<MechanicEvent>() },
      };
      for (int i = 0; i < 8; ++i) {
        int degrees = i * 45;
        mechanicData.mechanicPools[_strengthOfTheWardHeavyImpactPool].Add(new SpawnMechanicEvent() {
          referenceMechanicName = _strengthOfTheWardHeavyImpact,
          position = new Vector2(
              Mathf.Cos(Mathf.Deg2Rad * degrees) * _strengthOfTheWardHeavyImpactPosition,
              Mathf.Sin(Mathf.Deg2Rad * degrees) * _strengthOfTheWardHeavyImpactPosition),
        });
      }
      for (int i = 0; i < 4; ++i) {
        int degrees = i * 45;
        mechanicData.mechanicPools[_strengthOfTheWardSpiralThrustPool].Add(
            new SpawnMechanicEvent() {
              referenceMechanicName = _strengthOfTheWardSpiralThrust,
              position = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degrees) * _arenaRadius,
                                     Mathf.Sin(Mathf.Deg2Rad * degrees) * _arenaRadius),
              rotation = -degrees - 90,
            });
      }

      return mechanicData;
    }
  }
}
