using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be responsible for time management of various devices.
/// 
/// Design
/// ----------
/// Mode 1: Single Fire
/// On trigger, the class shall activate firing procedure.
/// 
/// Mode 2: Repeated Fire
/// On trigger, the class shall schedule an interval of for the firing solution until deactivated.
/// 
/// Mode 3: On Duration
/// For a duration of time, the class will maintain some kind of event.
/// 
/// </summary>
public class SystemScheduler {

	private float _firingDuration = 0.500f;

	private float _firingInterval = 0.50f;

	private float _coolDown;

	private bool _inFiringState;

	private int _numberOfFired;

	private float _firingTimePassed;

	private float _cooldownTimePassed;

	private float _totalFiringTimePassed;

	private Pattern _pattern;

	// Create a mangeable object.
	public SystemScheduler()
	{
		_pattern = Pattern.Repeating;

		_firingDuration = 2.0f;
		_firingInterval = 0.5f;
		_coolDown = 5;

		_inFiringState = false;
		_firingTimePassed = 0;
		_numberOfFired = 0;

		_cooldownTimePassed = 0;
		_totalFiringTimePassed = 0;
	}


	private SystemScheduler(float firingDuration, float firingInterval, float coolDown)
	{
		_pattern = Pattern.Repeating;

		_firingDuration = firingDuration;
		_firingInterval = firingInterval;
		_coolDown = coolDown;

		_inFiringState = false;
		_firingTimePassed = 0;
		_numberOfFired = 0;

		_cooldownTimePassed = 0;
		_totalFiringTimePassed = 0;
	}

	public static SystemScheduler CreateRepeatingScheduler(float firingDuration, float firingInterval, float coolDown)
	{
		return new SystemScheduler(firingDuration, firingInterval, coolDown);
	}

	public void RunScheduler(FireAction action)
	{
		if(_pattern == Pattern.Repeating)
		{
			FireRepeating(action);
		}

		if(_pattern == Pattern.Duration)
		{

		}

		if(_pattern == Pattern.SingleFire)
		{
			// Generally we probably don't need this.
		}
	}

	// Update is called once per frame.
	private void FireRepeating(FireAction action)
	{
		if (_inFiringState)
		{
			_firingTimePassed += Time.deltaTime;
			_totalFiringTimePassed += Time.deltaTime;
			//print("firing" + _firingTimePassed);

			if (_firingTimePassed >= _firingInterval)
			{
				//print("firing*" + _firingTimePassed);
				_firingTimePassed = 0;
				action();
			}

			if (_totalFiringTimePassed >= _firingDuration)
			{
				_firingTimePassed = 0;
				_totalFiringTimePassed = 0;
				_inFiringState = false;
			}
		}
		else
		{
			_cooldownTimePassed += Time.deltaTime;
			if (_cooldownTimePassed >= _coolDown)
			{
				_cooldownTimePassed = 0;
				_inFiringState = true;
			}
		}
	}

	public void ForceEnableFiring()
	{
		_inFiringState = true;
	}
}

public delegate void FireAction();

public enum Pattern
{
	/*
	 * Some sort of single shell weapon.
	 * Heavy weapons like missiles and torpedoes
	 * fall in this category.
	 */
	SingleFire,
	/*
	 * Repeating weapons, autmatic firing stuff.
	 * Like flak cannons, and standard vulcan
	 * guns and artillery.
	 */
	Repeating,
	/*
	 * Weapons that take time to fire, charge and cool down.
	 * Tractor beams, laser beams, and marine takeovers fall in this category.
	 */
	Duration
}
